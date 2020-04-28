using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuakeKanban.Data;

namespace QuakeKanban.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Project.Any())
                {
                    return;
                }

                context.Project.AddRange(
                    new Project
                    {
                        Name = "Issue tracking system",
                        Description = "Build an issue tracking system using .Net core Web API and Angular",
                        Tasks = new List<Task> {
                            new Task
                            {
                                Summary = "Create security pages",
                                Description = "Build pages where users can register/login to the system",
                                Status = TaskStatus.Done,
                                StoryPoints = 3
                            },
                            new Task
                            {
                                Summary = "Create Projects CRUD API",
                                Description = "Build an API where project records can be added, edited, deleted and listed",
                                Status = TaskStatus.InQA,
                                StoryPoints = 5
                            },
                            new Task
                            {
                                Summary = "Create Tasks CRUD API",
                                Description = "Build an API where task records can be added, edited, deleted and listed",
                                Status = TaskStatus.InQA,
                                StoryPoints = 8
                            },
                            new Task
                            {
                                Summary = "Build sprint board page",
                                Description = "Create a page where tasks can be organized into columns by their status",
                                Status = TaskStatus.InProgress,
                                StoryPoints = 13
                            },
                            new Task
                            {
                                Summary = "Implement issue relations",
                                Description = "Provide a way to relate issues to one another in various ways such as: blocks, is related to, duplicate",
                                Status = TaskStatus.Ready,
                                StoryPoints = 8
                            },
                            new Task
                            {
                                Summary = "Implement issue types",
                                Description = "Provide a way to classify issues into categories such as: feature, bug, refactoring",
                                Status = TaskStatus.Ready,
                                StoryPoints = 5
                            },
                            new Task
                            {
                                Summary = "Implement issue priority",
                                Description = "Provide a way to prioritize issues",
                                Status = TaskStatus.Ready,
                                StoryPoints = 5
                            }
                        }
                    },
                    new Project
                    {
                        Name = "COVID-19 Survival",
                        Description = "Survive the coronavirus pandemic",
                        Tasks = new List<Task> {
                            new Task
                            {
                                Summary = "Stockpile pasta",
                                Description = "Buy any pasta left on shelves",
                                Status = TaskStatus.Done,
                                StoryPoints = 5
                            },
                            new Task
                            {
                                Summary = "Stockpile toilet rolls",
                                Description = "Buy 12 x 12 pack rolls",
                                Status = TaskStatus.InQA,
                                StoryPoints = 3
                            },
                            new Task
                            {
                                Summary = "Listen to fake news",
                                Description = "Acquire as many wrong facts about fighting covid as possible",
                                Status = TaskStatus.InQA,
                                StoryPoints = 2
                            },
                            new Task
                            {
                                Summary = "Make home-made face mask",
                                Description = "Fail to find any proper masks in shops, use an old t-shirt to roll your own",
                                Status = TaskStatus.InQA,
                                StoryPoints = 5
                            },
                            new Task
                            {
                                Summary = "Keep social distance",
                                Description = "Make curves as you walk to avoid getting too close to people",
                                Status = TaskStatus.InProgress,
                                StoryPoints = 5
                            },
                            new Task
                            {
                                Summary = "Stay at home",
                                Description = "Self isolate, unlimited netflix and x-box",
                                Status = TaskStatus.InProgress,
                                StoryPoints = 5
                            },
                            new Task
                            {
                                Summary = "Pick up a new skill",
                                Description = "Juggling, building a tower of playing cards for instance",
                                Status = TaskStatus.Ready,
                                StoryPoints = 8
                            }
                        }
                    }
                );
                context.SaveChanges();
            }
        }
    }
}