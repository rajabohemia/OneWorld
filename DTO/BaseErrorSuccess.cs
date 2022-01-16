using System.Collections.Generic;

namespace OneWorld.DTO
{
    public class BaseErrorSuccess
    {
        public BaseErrorSuccess()
        {
            Success = false;
        }

        public BaseErrorSuccess(bool success)
        {
            Success = success;
        }

        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}