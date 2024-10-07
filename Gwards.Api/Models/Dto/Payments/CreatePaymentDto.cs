using Gward.Common.Enums;
using Gwards.DAL.Entities;

namespace Gwards.Api.Models.Dto.Payments;

public class CreatePaymentDto
{
    public UserEntity User { get; set; }
    public PostPaymentMethod PostPaymentMethod { get; set; }
}