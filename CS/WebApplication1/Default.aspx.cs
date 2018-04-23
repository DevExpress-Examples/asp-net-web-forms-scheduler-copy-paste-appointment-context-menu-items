using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraScheduler;

namespace WebApplication1 {
    public partial class Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void ObjectDataSourceResources_ObjectCreated(object sender, ObjectDataSourceEventArgs e) {
            if(Session["CustomResourceDataSource"] == null) {
                Session["CustomResourceDataSource"] = new CustomResourceDataSource(GetCustomResources());
            }
            e.ObjectInstance = Session["CustomResourceDataSource"];
        }

        BindingList<CustomResource> GetCustomResources() {
            BindingList<CustomResource> resources = new BindingList<CustomResource>();
            resources.Add(CreateCustomResource(1, "Max Fowler"));
            resources.Add(CreateCustomResource(2, "Nancy Drewmore"));
            resources.Add(CreateCustomResource(3, "Pak Jang"));
            return resources;
        }

        private CustomResource CreateCustomResource(int res_id, string caption) {
            CustomResource cr = new CustomResource();
            cr.ResID = res_id;
            cr.Name = caption;
            return cr;
        }

        public Random RandomInstance = new Random();
        private CustomAppointment CreateCustomAppointment(string subject, object resourceId, int status, int label) {
            CustomAppointment apt = new CustomAppointment();
            apt.Subject = subject;
            apt.OwnerId = resourceId;
            Random rnd = RandomInstance;
            int rangeInMinutes = 60 * 24;
            apt.StartTime = DateTime.Today + TimeSpan.FromMinutes(rnd.Next(0, rangeInMinutes));
            apt.EndTime = apt.StartTime + TimeSpan.FromMinutes(rnd.Next(0, rangeInMinutes / 4));
            apt.Status = status;
            apt.Label = label;
            return apt;
        }

        protected void ObjectDataSourceAppointment_ObjectCreated(object sender, ObjectDataSourceEventArgs e) {
            if(Session["CustomAppointmentDataSource"] == null) {
                Session["CustomAppointmentDataSource"] = new CustomAppointmentDataSource(GetCustomAppointments());
            }
            e.ObjectInstance = Session["CustomAppointmentDataSource"];
        }

        BindingList<CustomAppointment> GetCustomAppointments() {
            BindingList<CustomAppointment> appointments = new BindingList<CustomAppointment>(); ;
            CustomResourceDataSource resources = Session["CustomResourceDataSource"] as CustomResourceDataSource;
            if(resources != null) {
                foreach(CustomResource item in resources.Resources) {
                    string subjPrefix = item.Name + "'s ";
                    appointments.Add(CreateCustomAppointment(subjPrefix + "meeting", item.ResID, 2, 5));
                    appointments.Add(CreateCustomAppointment(subjPrefix + "travel", item.ResID, 3, 6));
                    appointments.Add(CreateCustomAppointment(subjPrefix + "phone call", item.ResID, 0, 10));
                }
            }
            return appointments;
        }

        protected void ASPxScheduler1_PopupMenuShowing(object sender, DevExpress.Web.ASPxScheduler.PopupMenuShowingEventArgs e) {
            e.Menu.ClientInstanceName = "AppointmentPopupMenu";
            e.Menu.ClientSideEvents.PopUp = "OnClientPopupMenuShowing";
            e.Menu.ClientSideEvents.ItemClick = "OnClientItemClick";

            if(e.Menu.MenuId == DevExpress.XtraScheduler.SchedulerMenuItemId.AppointmentMenu) {
                DevExpress.Web.MenuItem newItemCopy = new DevExpress.Web.MenuItem();
                newItemCopy.Name = "CopyAppointment";
                newItemCopy.Text = "Copy";
                newItemCopy.ItemStyle.Font.Bold = true;
                e.Menu.Items.Add(newItemCopy);
                e.Menu.JSProperties["cpMenuName"] = "AppointmentMenu";
            }
            if(e.Menu.MenuId == DevExpress.XtraScheduler.SchedulerMenuItemId.DefaultMenu) {
                DevExpress.Web.MenuItem newItemPaste = new DevExpress.Web.MenuItem();
                newItemPaste.Name = "PasteAppointment";
                newItemPaste.Text = "Paste";
                newItemPaste.ItemStyle.Font.Bold = true;
                e.Menu.Items.Insert(0, newItemPaste);
                e.Menu.JSProperties["cpMenuName"] = "DefaultMenu";
            }
        }

        protected void ASPxScheduler1_CustomCallback(object sender, DevExpress.Web.CallbackEventArgsBase e) {
            if(e.Parameter == "PasteAppointment" && hdCopiedAppointmentID.Value != "") {
                int copyAppointmentID = Convert.ToInt32(hdCopiedAppointmentID.Value);
                Appointment sourceAppointment = ASPxScheduler1.Storage.Appointments.GetAppointmentById(copyAppointmentID);
                if(sourceAppointment != null) {
                    Appointment newAppointment = ASPxScheduler1.Storage.CreateAppointment(sourceAppointment.Type);
                    newAppointment.Description = sourceAppointment.Description;
                    newAppointment.LabelKey = sourceAppointment.LabelKey;
                    newAppointment.Location = sourceAppointment.Location;
                    newAppointment.ResourceId = ASPxScheduler1.SelectedResource.Id; ;
                    newAppointment.Subject = sourceAppointment.Subject;
                    newAppointment.StatusKey = sourceAppointment.StatusKey;

                    newAppointment.Start = ASPxScheduler1.SelectedInterval.Start;
                    newAppointment.End = newAppointment.Start + sourceAppointment.Duration;

                    ASPxScheduler1.Storage.Appointments.Add(newAppointment);
                    ASPxScheduler1.JSProperties["cpCallBackParameter"] = "PasteAppointment";
                }
            }
        }
    }
}