using MediatR;

namespace Order.Domain;

public class Orders
{
   public  Guid id { get;  set; } = Guid.NewGuid();
   public Guid userId { get; set; }
   public List<Guid> ProductIds { get;  set; }
   public decimal Amount { get; set; }
   public string currency { get; set; } = "Rub";
   public string status { get;  set; } = "WaitingPayment";
   public DateTime created_at { get; private set; } = DateTime.UtcNow;
   public DateTime? cancelled_at { get;  set; }
   public DateTime? refunded_at { get;  set; }
   
}