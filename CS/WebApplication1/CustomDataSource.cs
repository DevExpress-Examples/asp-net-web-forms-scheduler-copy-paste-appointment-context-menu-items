using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication1 {
    public class CustomAppointmentDataSource {
        BindingList<CustomAppointment> appts;
        public CustomAppointmentDataSource(BindingList<CustomAppointment> parAppts) {
            if(parAppts == null)
                DevExpress.XtraScheduler.Native.Exceptions.ThrowArgumentNullException("Appointments");
            this.Appointments = parAppts;
        }
        public CustomAppointmentDataSource()
            : this(new BindingList<CustomAppointment>()) {

        }
        public BindingList<CustomAppointment> Appointments { 
            get { return appts; } 
            set {
                if(value is BindingList<CustomAppointment> && (value as BindingList<CustomAppointment>).Count > 0 ) {
                    appts = value;
                    for(int i = 0; i < appts.Count; i++) {
                        appts[i].Id = i;
                    }
                }
            } 
        }

        #region ObjectDataSource methods                        
        public CustomAppointment GetSourceAppointment(object apptId) {
            foreach(CustomAppointment item in Appointments) {
                if(Convert.ToInt32(item.Id) == Convert.ToInt32(apptId)) {
                    return item;
                }
            }
            return null;
        }

        public object InsertMethodHandler(CustomAppointment customAppt) {
            int lastId = 0;
            foreach (CustomAppointment item in Appointments)
	        {
                if (Convert.ToInt32(item.Id) > lastId) 
                    lastId = Convert.ToInt32(item.Id);		 
	        }
            customAppt.Id = lastId + 1;
            Appointments[Appointments.Count - 1].Id = customAppt.Id;
            return customAppt.Id;
        }
        public void DeleteMethodHandler(CustomAppointment customAppt) {
            CustomAppointment currentAppt = GetSourceAppointment(customAppt.Id);
            if(customAppt != null)
                Appointments.Remove(customAppt);
        }
        public void UpdateMethodHandler(CustomAppointment customAppt) {
            CustomAppointment currentAppt = GetSourceAppointment(customAppt.Id);
            if(customAppt != null) {
                currentAppt.AllDay = customAppt.AllDay;
                currentAppt.Description = customAppt.Description;
                currentAppt.EndTime = customAppt.EndTime;
                currentAppt.EventType = customAppt.EventType;
                currentAppt.Id = customAppt.Id;
                currentAppt.Label = customAppt.Label;
                currentAppt.Location = customAppt.Location;
                currentAppt.OwnerId = customAppt.OwnerId;
                currentAppt.RecurrenceInfo = customAppt.RecurrenceInfo;
                currentAppt.ReminderInfo = customAppt.ReminderInfo;
                currentAppt.StartTime = customAppt.StartTime;
                currentAppt.Status = customAppt.Status;
                currentAppt.Subject = customAppt.Subject;
            }
        }
        public IEnumerable SelectMethodHandler() {
            return Appointments;
        }
        #endregion
    }

    public class CustomResourceDataSource {
        BindingList<CustomResource> resources;
        public CustomResourceDataSource(BindingList<CustomResource> parResources) {
            if(parResources == null)
                DevExpress.XtraScheduler.Native.Exceptions.ThrowArgumentNullException("Resources");
            this.Resources = parResources;
        }
        public CustomResourceDataSource() : this(new BindingList<CustomResource>()) {

        }
        public BindingList<CustomResource> Resources { 
            get { return resources; } 
            set { resources = value; } 
        }

        #region ObjectDataSource methods
        public IEnumerable SelectMethodHandler() {
            return Resources;
        }
        #endregion
    }
}