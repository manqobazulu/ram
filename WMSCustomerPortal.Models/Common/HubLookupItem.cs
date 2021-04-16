
namespace WMSCustomerPortal.Models.Common {

    public class HubLookupItem {
        public string HubID { get; set; }
        public string Description { get; set; }

        public HubLookupItem() {

        }

        public HubLookupItem(string hubId, string description) {
            HubID = hubId;
            Description = description;
        }
    }
}
