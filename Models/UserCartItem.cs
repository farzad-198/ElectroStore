namespace ElectroStore.Models
{
    public class UserCartItem
    {
        //sabate kharid afrade login shodas
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity  { get; set; }
        public  Product Product { get; set; }
        public ApplicationUser User { get; set; }

    }
}
