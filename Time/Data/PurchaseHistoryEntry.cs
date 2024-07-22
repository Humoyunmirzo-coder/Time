namespace Time.Data
{
    public class PurchaseHistoryEntry
    {
        public string ProductName { get; set; }
        public string BuyerId { get; set; }
        public int Quantity { get; set; }
        public DateTime Timestamp { get; set; }

        public PurchaseHistoryEntry(string productName, string buyerId, int quantity, DateTime timestamp)
        {
            ProductName = productName;
            BuyerId = buyerId;
            Quantity = quantity;
            Timestamp = timestamp;
        }

        public override string ToString()
        {
            return $"[  {ProductName}  -  {BuyerId} - {Quantity} - {Timestamp}  ]";
        }
    }

}
