using Microsoft.AspNetCore.Mvc;
using PaymentsAPI.Models;
using PaymentsAPI.Services;
using System.Text.RegularExpressions;

namespace PaymentsAPI.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(PaymentService repo)
        {
            _paymentService = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var allPayments = await _paymentService.GetAllPaymentsAsync();
                return Ok(allPayments);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllPayments: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetPaymentById(Guid transactionId)
        {
            try
            {
                var allPaymentsByTransId = await _paymentService.GetPaymentByIdAsync(transactionId);
                if (allPaymentsByTransId == null)
                {
                    _logger.LogWarning($"Payment with ID: {transactionId} not found.");
                    return NotFound($"Payment with ID: {transactionId} not found.");
                }
                return Ok(allPaymentsByTransId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetPaymentById: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment([FromBody] Payment payment)
        {
            try
            {
                if (!ValidatePayment(payment))
                {
                    _logger.LogWarning("Invalid input parameters.");
                    return BadRequest("Invalid Request Parameters");
                }
                payment.TransactionID = Guid.NewGuid();
                await _paymentService.AddPaymentAsync(payment);
                return CreatedAtAction(
                    nameof(GetPaymentById),
                    new { transactionID = payment.TransactionID },
                    payment
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddPayment: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{transactionID}")]
        public async Task<IActionResult> UpdatePayment(Guid transactionID, [FromBody] Payment payment)
        {
            try
            {
                if (transactionID != payment.TransactionID)
                {
                    _logger.LogWarning("Transaction ID mismatch.");
                    return BadRequest("Transaction ID mismatch.");
                }

                if (!ValidatePayment(payment))
                {
                    _logger.LogWarning("Invalid input parameters.");
                    return BadRequest("Invalid input parameters.");
                }

                await _paymentService.UpdatePaymentAsync(payment);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdatePayment: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private bool ValidatePayment(Payment paymentCreds)
        {
            if (string.IsNullOrEmpty(paymentCreds.Name)) return false;

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(paymentCreds.Email)) return false;

            if (string.IsNullOrEmpty(paymentCreds.PhoneNumber) || !paymentCreds.PhoneNumber.All(char.IsDigit)) return false;

            if (string.IsNullOrEmpty(paymentCreds.ModeOfPayment)) return false;

            return true;
        }
    }
}