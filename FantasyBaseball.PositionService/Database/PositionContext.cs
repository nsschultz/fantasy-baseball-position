using FantasyBaseball.Common.Enums;
using FantasyBaseball.PositionService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FantasyBaseball.PositionService.Database
{
    /// <summary>The context object for players and their related entities.</summary>
    public class PositionContext : DbContext, IPositionContext
    {
        /// <summary>
        ///     Initializes a new instance of the Microsoft.EntityFrameworkCore.DbContext class using the specified options. 
        ///     The Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)
        ///     method will still be called to allow further configuration of the options.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public PositionContext(DbContextOptions<PositionContext> options) : base(options) { }

        /// <summary>A collection of child positions.</summary>
        public DbSet<AdditionalPositionEntity> AdditionalPositions { get; set; }

        /// <summary>A collection of positions.</summary>
        public DbSet<PositionEntity> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildPositionModel(modelBuilder.Entity<PositionEntity>());
            BuildAdditionalPositionModel(modelBuilder.Entity<AdditionalPositionEntity>());
        }

        private static void BuildAdditionalPositionModel(EntityTypeBuilder<AdditionalPositionEntity> builder)
        {
            builder.HasKey(a => new { a.ParentCode, a.ChildCode }).HasName("AdditionalPosition_PK");
            builder.HasOne(a => a.ChildPosition)
                .WithMany(p => p.ChildPositions)
                .HasForeignKey(a => a.ChildCode)
                .HasConstraintName("AdditionalPosition_ChildPosition_FK");
            builder.HasOne(a => a.ParentPosition)
                .WithMany(a => a.ParentPositions)
                .HasForeignKey(a => a.ParentCode)
                .HasConstraintName("AdditionalPosition_ParentPosition_FK");
             builder.HasData(
                 new AdditionalPositionEntity { ParentCode = "C"  , ChildCode = "UTIL" },
                 new AdditionalPositionEntity { ParentCode = "1B" , ChildCode = "CIF"  },
                 new AdditionalPositionEntity { ParentCode = "1B" , ChildCode = "IF"   },
                 new AdditionalPositionEntity { ParentCode = "1B" , ChildCode = "UTIL" },
                 new AdditionalPositionEntity { ParentCode = "2B" , ChildCode = "MIF"  },
                 new AdditionalPositionEntity { ParentCode = "2B" , ChildCode = "IF"   },
                 new AdditionalPositionEntity { ParentCode = "2B" , ChildCode = "UTIL" },
                 new AdditionalPositionEntity { ParentCode = "3B" , ChildCode = "CIF"  },
                 new AdditionalPositionEntity { ParentCode = "3B" , ChildCode = "IF"   },
                 new AdditionalPositionEntity { ParentCode = "3B" , ChildCode = "UTIL" },
                 new AdditionalPositionEntity { ParentCode = "SS" , ChildCode = "MIF"  },
                 new AdditionalPositionEntity { ParentCode = "SS" , ChildCode = "IF"   },
                 new AdditionalPositionEntity { ParentCode = "SS" , ChildCode = "UTIL" },
                 new AdditionalPositionEntity { ParentCode = "CIF", ChildCode = "IF"   },
                 new AdditionalPositionEntity { ParentCode = "CIF", ChildCode = "UTIL" },
                 new AdditionalPositionEntity { ParentCode = "MIF", ChildCode = "IF"   },
                 new AdditionalPositionEntity { ParentCode = "MIF", ChildCode = "UTIL" },
                 new AdditionalPositionEntity { ParentCode = "IF" , ChildCode = "UTIL" },
                 new AdditionalPositionEntity { ParentCode = "LF" , ChildCode = "OF"   },
                 new AdditionalPositionEntity { ParentCode = "LF" , ChildCode = "UTIL" },
                 new AdditionalPositionEntity { ParentCode = "CF" , ChildCode = "OF"   },
                 new AdditionalPositionEntity { ParentCode = "CF" , ChildCode = "UTIL" },
                 new AdditionalPositionEntity { ParentCode = "RF" , ChildCode = "OF"   },
                 new AdditionalPositionEntity { ParentCode = "RF" , ChildCode = "UTIL" },
                 new AdditionalPositionEntity { ParentCode = "OF" , ChildCode = "UTIL" },
                 new AdditionalPositionEntity { ParentCode = "DH" , ChildCode = "UTIL" },
                 new AdditionalPositionEntity { ParentCode = "SP" , ChildCode = "P"    },
                 new AdditionalPositionEntity { ParentCode = "RP" , ChildCode = "P"    }
             );
        }

        private static void BuildPositionModel(EntityTypeBuilder<PositionEntity> builder)  
        {
            builder.HasKey(b => b.Code).HasName("Position_PK");
            builder.Property(b => b.Code).HasMaxLength(3);
            builder.Property(b => b.FullName).HasMaxLength(20);
            builder.HasIndex(b => b.SortOrder).IsUnique();
            builder.HasData(
                new PositionEntity { Code = ""    , FullName = "Unknown"          , PlayerType = PlayerType.U, SortOrder = int.MaxValue },
                new PositionEntity { Code = "C"   , FullName = "Catcher"          , PlayerType = PlayerType.B, SortOrder = 0            },
                new PositionEntity { Code = "1B"  , FullName = "First Baseman"    , PlayerType = PlayerType.B, SortOrder = 1            },
                new PositionEntity { Code = "2B"  , FullName = "Second Baseman"   , PlayerType = PlayerType.B, SortOrder = 2            },
                new PositionEntity { Code = "3B"  , FullName = "Third Baseman"    , PlayerType = PlayerType.B, SortOrder = 3            },
                new PositionEntity { Code = "SS"  , FullName = "Shortstop"        , PlayerType = PlayerType.B, SortOrder = 4            },
                new PositionEntity { Code = "CIF" , FullName = "Corner Infielder" , PlayerType = PlayerType.B, SortOrder = 5            },
                new PositionEntity { Code = "MIF" , FullName = "Middle Infielder" , PlayerType = PlayerType.B, SortOrder = 6            },
                new PositionEntity { Code = "IF"  , FullName = "Infielder"        , PlayerType = PlayerType.B, SortOrder = 7            },
                new PositionEntity { Code = "LF"  , FullName = "Left Fielder"     , PlayerType = PlayerType.B, SortOrder = 8            },
                new PositionEntity { Code = "CF"  , FullName = "Center Feilder"   , PlayerType = PlayerType.B, SortOrder = 9            },
                new PositionEntity { Code = "RF"  , FullName = "Right Fielder"    , PlayerType = PlayerType.B, SortOrder = 10           },
                new PositionEntity { Code = "OF"  , FullName = "Outfielder"       , PlayerType = PlayerType.B, SortOrder = 11           },
                new PositionEntity { Code = "DH"  , FullName = "Designated Hitter", PlayerType = PlayerType.B, SortOrder = 12           },
                new PositionEntity { Code = "UTIL", FullName = "Utility"          , PlayerType = PlayerType.B, SortOrder = 13           },
                new PositionEntity { Code = "SP"  , FullName = "Starting Pitcher" , PlayerType = PlayerType.P, SortOrder = 100          },
                new PositionEntity { Code = "RP"  , FullName = "Relief Pitcher"   , PlayerType = PlayerType.P, SortOrder = 101          },
                new PositionEntity { Code = "P"   , FullName = "Pitcher"          , PlayerType = PlayerType.P, SortOrder = 102          }
            );
        }
    }
}