namespace BasicSettings.DataLayer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CustomeIdentityUser> ApplicationUser { get; set; }
        public DbSet<CustomeIdentityRole> CustomeIdentityRole { get; set; }
        public DbSet<UsersRoles> UsersRoles { get; set; }
        public DbSet<SystemTasks> SystemTasks { get; set; }
        public DbSet<RoleProfiles> RoleProfiles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UsersRoles>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
        }
    }
}
