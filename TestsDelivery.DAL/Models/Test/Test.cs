﻿namespace TestsDelivery.DAL.Models.Test
{
    public record Test : IdEntity<long>
    {
        public string Name { get; set; }
    }
}
