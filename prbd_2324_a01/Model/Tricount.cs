using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace prbd_2324_a01.Model
{
    public class Tricount : EntityBase<PridContext>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        [Required, ForeignKey(nameof(User))]
        public int Creator { get; set; }
        public int Id { get; set; }

        public virtual ICollection<User> Participants { get; set; } = new HashSet<User>();
        public virtual ICollection<Operation> Operations { get; set; } = new HashSet<Operation>();
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new HashSet<Subscription>();

        public Tricount(string title, string description, int creator, DateTime createdAt) {
            Title = title;
            Description = description;
            Creator = creator;
            CreatedAt = createdAt;
        }

        public Tricount() {
        }

        public static IQueryable<Tricount> GetAll() {
            return Context.Tricounts;
        }

        public static IQueryable<Tricount> GetAllFiltered(string filter) {
            var filtered = from t in Context.Tricounts
                           where t.Title.Contains(filter) ||
                                 t.Description.Contains(filter) ||
                                 t.Participants.Any(p => p.FullName.Contains(filter)) ||
                                 t.Operations.Any(o => o.Title.Contains(filter))
                           select t;
            return filtered;
        }


        public double GetTotalExpenses() {
            double totalExpenses = 0;
            foreach (Operation o in this.GetAllOperations()) {
                totalExpenses += o.Amount;
            }
            return totalExpenses;
        }

        public double GetMyExpenses(int id) {
            double myExpenses = 0;
            foreach (Operation o in this.GetOperationsById(id)) {
                myExpenses += o.Amount;
            }
            return myExpenses;
        }

        public void Add() {
            Context.Tricounts.Add(this);
            Context.SaveChanges();
        }

        public void Update() {
            Context.Tricounts.Update(this);
            Context.SaveChanges();
        }

        public IQueryable<Operation> GetAllOperations() {
            return Context.Operations.Where(o => o.Tricount == this.Id).OrderByDescending(o => o.OperationDate).OrderByDescending(o => o.Id);
        }

        public IQueryable<Operation> GetOperationsById(int id) {
            return Context.Operations.Where(o => o.Tricount == this.Id && o.Initiator == id).OrderByDescending(o => o.OperationDate);
        }

        public List<User> GetAllUsers() {
            List<User> users = new List<User>();
            foreach (Subscription sub in Context.Subscriptions.Where(s => s.TricountId == this.Id)) {
                users.Add(sub.User);
            }
            return users;
        }

        public Operation GetLastOperation() {
            return this.GetAllOperations().FirstOrDefault();
        }

        public Operation GetFirstOperation() {
            return this.GetAllOperations().LastOrDefault();
        }

        public List<User> GetAllParticipantsExpectCurrent(User CurrentUser) {
            List<User> users = new List<User>();
            foreach (Subscription sub in Context.Subscriptions.Where(s => s.TricountId == this.Id)) {
                if (sub.UserId != CurrentUser.Id) {
                    users.Add(Context.Users.Find(sub.UserId));
                }
            }
            return users;
        }

        public List<int> GetParticipantsIds() {
            return Context.Subscriptions
                          .Where(s => s.TricountId == Id)
                          .Select(s => s.User.Id)
                          .ToList();
        }

        public Dictionary<int, double> CalculateBalances() {

            var participantsIds = GetParticipantsIds();
            var balances = new Dictionary<int, double>();

            foreach (var participantId in participantsIds) {
                balances[participantId] = 0.0;
            }
            foreach (var operation in GetAllOperations()) {
                var shares = operation.GetParticipantShares();

                foreach (var share in shares) {
                    balances[share.Key] -= share.Value; // Part imputée à chaque participant
                }
                balances[operation.Initiator] += operation.Amount; // Montant total imputé au payeur
            }
            return balances;
        }

        public void Delete() {
            Context.Tricounts.Remove(this);
            Context.SaveChanges();
        }
    }
}
