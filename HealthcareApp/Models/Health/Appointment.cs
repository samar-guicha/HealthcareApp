using System;
using System.Collections.Generic;

namespace HealthcareApp.Models.Health;

public partial class Appointment
{
    public int Id { get; set; }
    public string Location { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int IdDoctor { get; set; }

    public virtual Doctor? IdDoctorNavigation { get; set; } = null!;
}
