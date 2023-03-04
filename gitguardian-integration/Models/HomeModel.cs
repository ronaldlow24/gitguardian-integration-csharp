namespace gitguardian_integration.Models
{

    public class GitGuardianResponseModel
    {
        public int policy_break_count { get; set; }
        public List<string> policies { get; set; }
        public List<GitGuardianPolicyDetailsModel> policy_breaks { get; set; }
    }

    public class GitGuardianPolicyDetailsModel
    {
        public string type { get; set; }
        public string policy { get; set; }
        public List<GitGuardianMatchModel> matches { get; set; }

    }

    public class GitGuardianMatchModel
    {
        public string type { get; set; }
        public string match { get; set; }

    }
}