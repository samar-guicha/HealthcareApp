using System;
using System.Collections.Generic;

namespace HealthcareApp.Models.Health;

public partial class Doctor
{
    public Doctor()
    {
        Appointments = new HashSet<Appointment>();
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Specialization { get; set; } = null!;
    public string LicenseNumber { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
