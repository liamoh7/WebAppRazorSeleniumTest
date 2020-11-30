using System;

namespace RazorUITests.Services
{
    public class HomeService : IHomeService
    {
        public string GoHome()
        {
            return "Real Home Service: Go Home";
        }
    }
}