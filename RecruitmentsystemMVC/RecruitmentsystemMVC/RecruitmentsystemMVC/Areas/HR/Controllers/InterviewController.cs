using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace RecruitmentsystemMVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class InterviewController : BaseHRController
    {
        private readonly IApiService _apiService;

        public InterviewController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: HR/Interview/Index (Default)
        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }

        // GET: HR/Interview/List
        public async Task<IActionResult> List()
        {
            try
            {
                // Fetch from HR API - plural 'Interviews'
                var interviews = await _apiService.GetAsync<List<InterviewResponseDTO>>("Interviews");
                
                if (interviews == null)
                {
                    TempData["Error"] = "Connection to API failed. Please try again later.";
                    return View(new List<InterviewResponseDTO>());
                }

                return View(interviews);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Application Error: " + ex.Message;
                return View(new List<InterviewResponseDTO>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id, int? applicationId)
        {
            var hrId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var hrName = HttpContext.Session.GetString("UserName") ?? "Current HR";

            // Populate Applications Dropdown
            var apps = await _apiService.GetAsync<List<ApplicationResponseDTO>>("Applications") ?? new List<ApplicationResponseDTO>();
            var applicationList = apps.Select(a => new
            {
                ApplicationId = a.ApplicationId,
                DisplayName = $"{a.CandidateName} - {a.JobTitle}"
            }).ToList();

            ViewBag.Applications = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(applicationList, "ApplicationId", "DisplayName");

            // Populating Interviewers (Users with Role HR/Admin)
            var interviewers = new List<dynamic> { new { UserId = hrId, UserName = hrName } };
            try
            {
                var users = await _apiService.GetAsync<List<Newtonsoft.Json.Linq.JObject>>("User");
                if (users != null && users.Count > 0)
                {
                    interviewers = users.Select(u => new { 
                        UserId = u.Value<int>("userId"), 
                        UserName = u.Value<string>("userName") 
                    }).ToList<dynamic>();
                }
            } catch { /* Fallback to current HR only */ }
            
            ViewBag.Interviewers = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(interviewers, "UserId", "UserName");

            if (id == null || id == 0)
            {
                return View(new InterviewDTO 
                { 
                    InterviewId = 0,
                    ApplicationId = applicationId ?? 0,
                    InterviewDate = DateTime.Now.AddDays(1).Date.AddHours(10),
                    Result = "Pending", 
                    Mode = "Online", 
                    RoundNo = 1,
                    InterviewerId = hrId 
                });
            }

            var interview = await _apiService.GetAsync<InterviewDTO>($"Interviews/{id}");
            if (interview == null) return NotFound();
            
            return View(interview);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(InterviewDTO model)
        {
            if (ModelState.IsValid)
            {
                if (model.InterviewerId == 0)
                    model.InterviewerId = HttpContext.Session.GetInt32("UserId") ?? 0;

                ApiResponse response;
                if (model.InterviewId == 0)
                {
                    var createDto = new InterviewCreateDTO
                    {
                        ApplicationId = model.ApplicationId,
                        InterviewerId = model.InterviewerId,
                        InterviewDate = model.InterviewDate,
                        Mode = model.Mode,
                        RoundNo = model.RoundNo,
                        Feedback = model.Feedback
                    };

                    response = await _apiService.PostAsync("Interviews", createDto);
                }
                else
                {
                    // Map string result to Enum
                    InterviewResult resultEnum = InterviewResult.Pending;
                    if (model.Result == "Selected" || model.Result == "Passed") resultEnum = InterviewResult.Selected;
                    else if (model.Result == "Rejected" || model.Result == "Failed") resultEnum = InterviewResult.Rejected;

                    var updateDto = new InterviewUpdateDTO
                    {
                        InterviewId = model.InterviewId,
                        InterviewDate = model.InterviewDate,
                        Mode = model.Mode,
                        RoundNo = model.RoundNo,
                        Feedback = model.Feedback,
                        Result = resultEnum
                    };

                    // API Update is PUT api/Interviews (no ID in URL)
                    response = await _apiService.PutAsync("Interviews", updateDto);
                }

                if (response.IsSuccess)
                {
                    TempData["Success"] = model.InterviewId == 0 ? "Interview scheduled successfully!" : "Interview updated successfully!";
                    return RedirectToAction(nameof(List));
                }
                ModelState.AddModelError("", response.Message);
            }
            
            // Reload dropdowns if failed
            var apps = await _apiService.GetAsync<List<ApplicationResponseDTO>>("Applications") ?? new List<ApplicationResponseDTO>();
            ViewBag.Applications = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(apps.Select(a => new { a.ApplicationId, DisplayName = $"{a.CandidateName} - {a.JobTitle}" }), "ApplicationId", "DisplayName");
            
            var hrId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var hrName = HttpContext.Session.GetString("UserName") ?? "Current HR";
            ViewBag.Interviewers = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<dynamic> { new { UserId = hrId, UserName = hrName } }, "UserId", "UserName");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Conduct(int id)
        {
            // Conduct view is basically Upsert but focused on Result/Feedback
            var interview = await _apiService.GetAsync<InterviewDTO>($"Interviews/{id}");
            if (interview == null) return NotFound();
            
            ViewData["IsConducting"] = true;
            return View("Upsert", interview);
        }

        [HttpGet]
        public async Task<IActionResult> GetDetails(int id)
        {
            try
            {
                var interview = await _apiService.GetAsync<InterviewResponseDTO>($"Interviews/{id}");
                if (interview == null) return Json(new { success = false, message = "Interview not found" });
                return Json(new { success = true, data = interview });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _apiService.DeleteAsync($"Interviews/{id}");
                if (response.IsSuccess)
                {
                    return Json(new { success = true, message = "Interview record has been deleted successfully." });
                }
                return Json(new { success = false, message = response.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Could not delete record: " + ex.Message });
            }
        }
    }
}
