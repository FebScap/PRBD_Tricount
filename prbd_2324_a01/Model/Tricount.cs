﻿using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace prbd_2324_a01.Model;

public class Tricount : EntityBase<PridContext>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsNew { get; set; } = false;

    [Required, ForeignKey(nameof(User))]
    public int Creator {  get; set; }
    public int Id { get; set; }

    public virtual ICollection<User> Participants { get; set; } = new HashSet<User>();
    public virtual ICollection<Operation> Operations { get; set; } = new HashSet<Operation>();

    public Tricount(string title, string description, int creator) {
        Title = title;
        Description = description;
        Creator = creator;
    }

    public Tricount() {
        IsNew = true;
    }

    public static IQueryable<Tricount> GetAll() {
        return Context.Tricounts;
    }

    public static IQueryable<Tricount> GetAllFiltered(string Filter) {
        var filtered = from t in Context.Tricounts
                       where t.Title.Contains(Filter) || t.Description.Contains(Filter)
                       select t;
        return filtered;
    }

    public double GetTotalExpenses() {
        double totalExpenses = 0;
        foreach (Operation o in this.GetAllOperations()) {
            totalExpenses += o.Amount;
        }
        return Math.Round(totalExpenses, 2);
    }

    internal double GetMyExpenses(int id) {
        double myExpenses = 0;
        foreach (Operation o in this.GetOperationsById(id)) {
            myExpenses += o.Amount;
        }
        return Math.Round(myExpenses, 2);
    }

    public void Add() {
        Context.Tricounts.Add(this);
        Context.SaveChanges();
    }

    public IQueryable<Operation> GetAllOperations() {
        return Context.Operations.Where(o => o.Tricount == this.Id).OrderByDescending(o => o.OperationDate);
    }
    public IQueryable<Operation> GetOperationsById(int id) {
        return Context.Operations.Where(o => o.Tricount == this.Id && o.Initiator == id).OrderByDescending(o => o.OperationDate);
    }

    public Operation GetLastOperation() {
        return this.GetAllOperations().FirstOrDefault();
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
}