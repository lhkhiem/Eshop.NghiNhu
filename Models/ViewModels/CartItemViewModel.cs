using Models.EF;
using System;

namespace Models.ViewModels
{
    [Serializable]
    public class CartItemViewModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}