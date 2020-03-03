using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using Xunit;

namespace FluentValidation_Tests
{
    public class FluentValidationTests
    {
        [Fact]
        public void TestCertificateCreateCommand()
        {
            Create certificate = new Create();
            CreateValidator validator = new CreateValidator();
            ValidationResult result  = validator.Validate(certificate);
        }
    }

    public interface ICommand
    {
        Guid Id { get; }
    }

    [Serializable]
    public abstract class Command : ICommand
    {
        public System.Guid Id { get; private set; }

        public Command()
        {
            Id = Guid.NewGuid();
        }

        public Command(Guid id)
        {
            Id = id;
        }
    }
    public enum UserType
    {
        None = 0,
        Operator = 1,
        PCU = 2,
        CertifyingAgent = 3,
        DMO = 4,
        IEC = 5,
        RI = 9,
        PCCB_S4 = 10,
        PCUSuperUser = 11,
        DMOGIP = 12
    }

    public enum SourceOrigin
    {
        API = 1,
        GUI = 2,
        MIGR = 3
    }

    public class CommandHeader
    {
        public string UserId { get; set; }
        public UserType UserType { get; set; }
        public SourceOrigin SourceOrigin { get; set; }

        public CommandHeader()
        {
            UserId = string.Empty;
            UserType = UserType.None;
        }
    }

    [Serializable]
    public abstract class CommandBase : Command
    {
        public CommandHeader Header { get; set; }

        public byte[] Version { get; set; }

        public CommandBase()
        {
            Header = new CommandHeader();
        }
    }

    [Serializable]
    public abstract class CertificateCommandBase : CommandBase
    {
        public Guid CertificateId { get; set; }
        public Dictionary<Guid, byte[]> CertificateIdsWithVersion { get; set; }
    }

    [Serializable]
    public class Create : CertificateCommandBase
    {
        public int RequestorId { get; set; }
        public string RequestorEnterpriseNumber { get; set; }
        public string ExternalReference { get; set; }

        public int ProductId { get; set; }
        public int ProductCategoryId { get; set; }
        public int GrammarId { get; set; }
        public int CountryId { get; set; }
    }

    public class CreateValidator : AbstractValidator<Create>
    {
        public CreateValidator()
        {
            RuleFor(x => x.RequestorId).NotEmpty();
            RuleFor(x => x.RequestorEnterpriseNumber).Matches("(^$)|(^\\d{10}$)");
            RuleFor(x => x.CertificateId).NotEmpty();
            RuleFor(x => x.ExternalReference).Length(0, 64);
            RuleFor(x => x.CountryId).GreaterThan(0);
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.ProductCategoryId).GreaterThan(0);
            RuleFor(x => x.GrammarId).GreaterThan(0);

        }
    }
}
