namespace Membership.API.Exceptions
{
    public class MembershipDomainException : Exception
    {
        public MembershipDomainException()
        {

        }
         public MembershipDomainException(string message):base(message)
        {

        }
        public MembershipDomainException(string message,Exception exception):base(message,exception)
        {

        }
    }
}
public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult(object error)
        : base(error)
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}
