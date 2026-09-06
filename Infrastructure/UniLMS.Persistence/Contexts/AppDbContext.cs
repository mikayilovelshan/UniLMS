using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniLMS.Domain.Entities;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Persistence.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Cafedra> Cafedras { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Group> Groups { get; set; }

        public DbSet<CourseOffering> CourseOfferings { get; set; }
        public DbSet<CourseSchedule> CourseSchedules { get; set; }

        public DbSet<Semester> Semesters { get; set; }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<StudentGrade> StudentGrades { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model
                         .GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            modelBuilder.Entity<Faculty>().HasQueryFilter(f => !f.IsDeleted);

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.Property(x => x.Name)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.Code)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.HasIndex(x => x.Code)
                    .IsUnique();

            
                entity.HasMany(x => x.Cafedras)
                    .WithOne(x => x.Faculty)
                    .HasForeignKey(x => x.FacultyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(x => x.Specialities)
                    .WithOne(x => x.Faculty)
                    .HasForeignKey(x => x.FacultyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });



            modelBuilder.Entity<Cafedra>(entity =>
            {
                entity.Property(x => x.Name)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.Code)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.HasIndex(x => x.Code)
                    .IsUnique();

         
                entity.HasOne(x => x.Faculty)
                    .WithMany(x => x.Cafedras)
                    .HasForeignKey(x => x.FacultyId)
                    .OnDelete(DeleteBehavior.Restrict);

        
                entity.HasMany(x => x.Specialities)
                    .WithOne(x => x.Cafedra)
                    .HasForeignKey(x => x.CafedraId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(x => x.Courses)
                    .WithOne(x => x.Cafedra)
                    .HasForeignKey(x => x.CafedraId)
                    .OnDelete(DeleteBehavior.Restrict);
            });




            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.Property(x => x.Name)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.Code)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.HasIndex(x => x.Code)
                    .IsUnique();


                entity.HasOne(x => x.Faculty)
                    .WithMany(x => x.Specialities)
                    .HasForeignKey(x => x.FacultyId)
                    .OnDelete(DeleteBehavior.Restrict);



                entity.HasOne(x => x.Cafedra)
                    .WithMany(x => x.Specialities)
                    .HasForeignKey(x => x.CafedraId)
                    .OnDelete(DeleteBehavior.Restrict);
            });



            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(x => x.Name)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.Code)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(x => x.Description)
                    .HasMaxLength(1000);

                entity.HasIndex(x => x.Code)
                    .IsUnique();


 
                entity.HasOne(x => x.Cafedra)
                    .WithMany(x => x.Courses)
                    .HasForeignKey(x => x.CafedraId)
                    .OnDelete(DeleteBehavior.Restrict);


  
                entity.HasMany(x => x.Specialities)
                    .WithMany(x => x.Courses)
                    .UsingEntity<Dictionary<string, object>>(
                        "CourseSpeciality",

                        right => right
                            .HasOne<Speciality>()
                            .WithMany()
                            .HasForeignKey("SpecialityId")
                            .OnDelete(DeleteBehavior.Restrict),

                        left => left
                            .HasOne<Course>()
                            .WithMany()
                            .HasForeignKey("CourseId")
                            .OnDelete(DeleteBehavior.Restrict),

                        join =>
                        {
                            join.HasKey("CourseId", "SpecialityId");
                        });
            });



            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(x => x.Code)
                    .HasMaxLength(30)
                    .IsRequired();


       
                entity.HasOne(x => x.Speciality)
                    .WithMany()
                    .HasForeignKey(x => x.SpecialityId)
                    .OnDelete(DeleteBehavior.Restrict);


         
                entity.HasMany(x => x.Students)
                    .WithOne(x => x.Group)
                    .HasForeignKey(x => x.GroupId)
                    .OnDelete(DeleteBehavior.Restrict);
            });



            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(x => x.StudentNumber)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(x => x.GPA)
                    .HasPrecision(5, 2);

                entity.HasIndex(x => x.StudentNumber)
                    .IsUnique();

                entity.HasIndex(x => x.AppUserId)
                    .IsUnique();


        
                entity.HasOne(x => x.AppUser)
                    .WithOne()
                    .HasForeignKey<Student>(x => x.AppUserId)
                    .OnDelete(DeleteBehavior.Restrict);


    
                entity.HasOne(x => x.Group)
                    .WithMany(x => x.Students)
                    .HasForeignKey(x => x.GroupId)
                    .OnDelete(DeleteBehavior.Restrict);
            });



            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.Property(x => x.ScientificDegree)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasIndex(x => x.AppUserId)
                    .IsUnique();


                entity.HasOne(x => x.AppUser)
                    .WithOne()
                    .HasForeignKey<Teacher>(x => x.AppUserId)
                    .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(x => x.Cafedra)
                    .WithMany()
                    .HasForeignKey(x => x.CafedraId)
                    .OnDelete(DeleteBehavior.Restrict);
            });




            modelBuilder.Entity<Semester>(entity =>
            {
                entity.Property(x => x.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasIndex(x => x.Name)
                    .IsUnique();
            });


            modelBuilder.Entity<CourseOffering>(entity =>
            {
                entity.Property(x => x.RoomCode)
                    .HasMaxLength(50)
                    .IsRequired();


          
                entity.HasOne(x => x.Group)
                    .WithMany()
                    .HasForeignKey(x => x.GroupId)
                    .OnDelete(DeleteBehavior.Restrict);


                
                entity.HasOne(x => x.Course)
                    .WithMany()
                    .HasForeignKey(x => x.CourseId)
                    .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(x => x.Teacher)
                    .WithMany()
                    .HasForeignKey(x => x.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(x => x.Semester)
                    .WithMany()
                    .HasForeignKey(x => x.SemesterId)
                    .OnDelete(DeleteBehavior.Restrict);


                entity.HasIndex(x => new
                {
                    x.GroupId,
                    x.CourseId,
                    x.SemesterId
                })
                .IsUnique();
            });


            modelBuilder.Entity<CourseSchedule>(entity =>
            {
                entity.HasOne(x => x.CourseOffering)
                    .WithMany()
                    .HasForeignKey(x => x.CourseOfferingId)
                    .OnDelete(DeleteBehavior.Restrict);


                entity.HasIndex(x => new
                {
                    x.CourseOfferingId,
                    x.DayOfWeek,
                    x.StartTime
                })
                .IsUnique();
            });


            modelBuilder.Entity<ExamResult>(entity =>
            {
                entity.Property(x => x.Score)
                    .HasPrecision(5, 2);


                entity.HasOne(x => x.Student)
                    .WithMany()
                    .HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(x => x.CourseOffering)
                    .WithMany()
                    .HasForeignKey(x => x.CourseOfferingId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(x => new
                {
                    x.StudentId,
                    x.CourseOfferingId,
                    x.ExamType
                })
                .IsUnique();
            });

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasOne(x => x.Student)
                    .WithMany()
                    .HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(x => x.CourseSchedule)
                    .WithMany()
                    .HasForeignKey(x => x.CourseScheduleId)
                    .OnDelete(DeleteBehavior.Restrict);



                entity.HasIndex(x => new
                {
                    x.StudentId,
                    x.CourseScheduleId
                })
                .IsUnique();
            });


            modelBuilder.Entity<StudentGrade>(entity =>
            {
                entity.Property(x => x.DailyScore)
                    .HasPrecision(5, 2);

                entity.Property(x => x.AttendanceScore)
                    .HasPrecision(5, 2);

                entity.Property(x => x.IndependentWorkScore)
                    .HasPrecision(5, 2);

                entity.Property(x => x.ColloquiumScore)
                    .HasPrecision(5, 2);

                entity.Property(x => x.EntryScore)
                    .HasPrecision(5, 2);

                entity.Property(x => x.FinalExamScore)
                    .HasPrecision(5, 2);

                entity.Property(x => x.TotalScore)
                    .HasPrecision(5, 2);

                entity.Property(x => x.LetterGrade)
                    .HasMaxLength(5)
                    .IsRequired();


                entity.HasOne(x => x.Student)
                    .WithMany()
                    .HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(x => x.CourseOffering)
                    .WithMany()
                    .HasForeignKey(x => x.CourseOfferingId)
                    .OnDelete(DeleteBehavior.Restrict);


            
                entity.HasIndex(x => new
                {
                    x.StudentId,
                    x.CourseOfferingId
                })
                .IsUnique();
            });



            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(x => x.FirstName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.LastName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.ProfilePhotoUrl)
                    .HasMaxLength(500);

                entity.Property(x => x.RefreshToken)
                    .HasMaxLength(500);
            });
        }
    }
}