using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.DTOs;
using Store.Application.Services.Contract;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class PaymentController(IOperationService operationService) : Controller
    {
        [Authorize]
        [HttpGet]
        public IActionResult Index(PaymentViewModel model) => View(model);

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaymentOperation(PaymentViewModel model)
        {
            var result = await operationService.SaveOperation(model.TotalAmount, model.ReceivedAmount, User);

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
        public IActionResult PaymentHistory() => View();


    }
}
