using MySql.Data.MySqlClient;
using PaymentsAPI.Models;
using System.Data;

namespace PaymentsAPI.Services
{
    public class PaymentService
    {
        private readonly string _connectionString;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IConfiguration configuration, ILogger<PaymentService> logger)
        {
            _connectionString = configuration.GetConnectionString("MySQLConnection");
            _logger = logger;
        }

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            var payments = new List<Payment>();

            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();
                var command = new MySqlCommand("SELECT * FROM Payments", conn);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    payments.Add(new Payment
                    {
                        TransactionID = reader.GetGuid("TransactionID"),
                        Name = reader.GetString("Name"),
                        Email = reader.GetString("Email"),
                        PhoneNumber = reader.GetString("PhoneNumber"),
                        ModeOfPayment = reader.GetString("ModeOfPayment")
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching payments: {ex.Message}");
            }

            return payments;
        }

        public async Task<Payment> GetPaymentByIdAsync(Guid id)
        {
            Payment payment = new();

            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();
                var command = new MySqlCommand("SELECT * FROM Payments WHERE TransactionID = @id", conn);
                command.Parameters.AddWithValue("@id", id);
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    payment = new Payment
                    {
                        TransactionID = reader.GetGuid("TransactionID"),
                        Name = reader.GetString("Name"),
                        Email = reader.GetString("Email"),
                        PhoneNumber = reader.GetString("PhoneNumber"),
                        ModeOfPayment = reader.GetString("ModeOfPayment")
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching payment by ID: {ex.Message}");
            }

            return payment;
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();
                var command = new MySqlCommand("INSERT INTO Payments (TransactionID, Name, Email, PhoneNumber, ModeOfPayment) VALUES (@transactionID, @name, @email, @phoneNumber, @modeOfPayment)", conn);
                command.Parameters.AddWithValue("@transactionID", payment.TransactionID);
                command.Parameters.AddWithValue("@name", payment.Name);
                command.Parameters.AddWithValue("@email", payment.Email);
                command.Parameters.AddWithValue("@phoneNumber", payment.PhoneNumber);
                command.Parameters.AddWithValue("@modeOfPayment", payment.ModeOfPayment);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding payment: {ex.Message}");
            }
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                var query = @"
                            UPDATE Payments 
                            SET Name = @name, 
                                Email = @email, 
                                PhoneNumber = @phoneNumber, 
                                ModeOfPayment = @modeOfPayment 
                            WHERE TransactionID = @transactionID";

                var command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@transactionID", payment.TransactionID);
                command.Parameters.AddWithValue("@name", payment.Name);
                command.Parameters.AddWithValue("@email", payment.Email);
                command.Parameters.AddWithValue("@phoneNumber", payment.PhoneNumber);
                command.Parameters.AddWithValue("@modeOfPayment", payment.ModeOfPayment);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                {
                    _logger.LogWarning($"No payment found with TransactionID: {payment.TransactionID}");
                    throw new KeyNotFoundException("Payment not found.");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating payment: {ex.Message}");
            }
        }
    }
}