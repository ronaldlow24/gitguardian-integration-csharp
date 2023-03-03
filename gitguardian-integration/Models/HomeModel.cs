namespace gitguardian_integration.Models
{
    public enum GitGuardianValidity
    {
        no_checker,
        valid,
        invalid,
        failed_to_check,
        unknown,
    }


    public class GitGuardianResponseModel
    {
        public int policy_break_count { get; set; }
        public string policies { get; set; }
        public List<GitGuardianPolicyDetailsModel> policy_breaks { get; set; }
    }

    public class GitGuardianPolicyDetailsModel
    {
        public string type { get; set; }
        public string policy { get; set; }
        public GitGuardianValidity? validity { get; set; }
        public List<GitGuardianMatchModel> matches { get; set; }

    }

    public class GitGuardianMatchModel
    {
        public string type { get; set; }
        public string match { get; set; }

    }
}