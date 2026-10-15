using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.DTOs;
using Store.Application.Services.Contract;
using Store.Web.Models;
using System.Security.Claims;

namespace Store.Web.Controllers
{
    public class PaymentController(IOperationService operationService) : Controller
    {
        private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


        [Authorize]
        [HttpGet]
        public IActionResult Index(PaymentViewModel model) => View(model);

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaymentOperation(PaymentViewModel model)
        {
            var result = await operationService.SaveOperation(model.TotalAmount, model.ReceivedAmount, UserId);

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View(nameof(Index), model);
            }

            var castResult = result.Data as ResponseOperationDTO;
            var modelOperation = new OperationViewModel
            {
                TotalAmount = castResult!.TotalAmount,
                ReceivedAmount = castResult!.ReceivedAmount,
                ChangeAmount = castResult!.ChangeAmount,
                ChangeBreakdown = castResult!.ChangeBreakdown
            };

            return View("Details", modelOperation);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await operationService.SearchOperation(id);

            if (result is null) return NotFound();

            var model = new OperationViewModel
            {
                TotalAmount = result.TotalAmount,
                ReceivedAmount = result.ReceivedAmount,
                ChangeAmount = result.ChangeAmount,
                ChangeBreakdown = result.ChangeBreakdown
            };

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> History()
        {
            var result = await operationService.HistoryOperationForUser(UserId);


            var model = result.Select(group => new HistoryOperationViewModel
            {
                Date = group.Date,
                Operations = group.Operations.Select(o => new OperationViewModel
                {
                    Id = o.Id,
                    TotalAmount = o.TotalAmount,
                    ReceivedAmount = o.ReceivedAmount,
                    ChangeAmount = o.ChangeAmount,
                    ChangeBreakdown = o.ChangeBreakdown
                }).ToList()
            }).ToList();

            return View(model);
        }
    }
}
