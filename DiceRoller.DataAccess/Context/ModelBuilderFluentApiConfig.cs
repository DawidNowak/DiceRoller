using DiceRoller.DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace DiceRoller.DataAccess.Context
{
    public static class ModelBuilderFluentApiConfig
    {
        public static ModelBuilder ConfigureGame(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsRequired();
            });

            return modelBuilder;
        }

        public static ModelBuilder ConfugureDice(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dice>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Dice>()
                .HasOne(s => s.Game)
                .WithMany(u => u.Dice)
                .IsRequired();

            modelBuilder.Entity<Dice>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });

            return modelBuilder;
        }

        public static ModelBuilder ConfugureDiceWall(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiceWall>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<DiceWall>()
                .HasOne(e => e.Dice)
                .WithMany(s => s.Walls)
                .IsRequired();

            modelBuilder.Entity<DiceWall>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });

            return modelBuilder;
        }
    }
}
