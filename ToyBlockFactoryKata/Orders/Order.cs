using System;
using System.Collections.Generic;

namespace ToyBlockFactoryKata.Orders
{
    public record Order
    {
        public Order(string customerName, string customerAddress)
            : this(customerName, customerAddress, DateTime.Today.AddDays(7))
        {
        }

        public Order(string customerName, string customerAddress, DateTime date)
        {
            Name = customerName;
            Address = customerAddress;
            DueDate = date;
        }

        public string Name { get; }
        public string Address { get; }
        public string OrderId { get; init; }
        public Dictionary<Block, int> BlockList { get; } = new();
        public DateTime DueDate { get; }

        internal Dictionary<Shape, int> shapeQuantities { get; } = new();


        public void AddBlock(Shape shape, Colour colour, int quantity)
        {
            var block = new Block(shape, colour);
            if (BlockList.ContainsKey(block))
                BlockList[block] += quantity;
            else
                BlockList.Add(block, quantity);

            AddShapeQuantity(block.Shape);
        }

        private void AddShapeQuantity(Shape blockShape)
        {
            if (shapeQuantities.TryGetValue(blockShape, out var quantity))
                shapeQuantities[blockShape] = ++quantity;
            else
                shapeQuantities.Add(blockShape, 1);
        }
    }
}