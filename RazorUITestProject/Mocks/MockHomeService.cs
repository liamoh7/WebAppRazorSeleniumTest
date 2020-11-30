using System;
using RazorUITests.Services;

namespace RazorUITestProject.Mocks
{
    public class MockHomeService : IHomeService
    {
        public string GoHome()
        {
            return "Fake Home Service: Goto Fake Home";
        }
    }
}