using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventPlannerServer
{
    public class AuthOptions
    {
        public const string ISSUER = "EventPlannerServer"; 
        public const string AUDIENCE = "EventPlannerClient"; 
        const string KEY = "am12148!opqweasdsadsxvscsdfw2312412!!!sdasdsfdgdsgdsgfsdfsd";   
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
