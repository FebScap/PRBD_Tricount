﻿using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace prbd_2324_a01.Model;

[Keyless]
public class Repartition : EntityBase<PridContext>
{
    public int Weight { get; set; }

    [Required, ForeignKey(nameof(Operation))]
    public int Operation { get; set; }

    [Required, ForeignKey(nameof(User))]
    public int User { get; set; }

    public Repartition(int weight, int operation, int user) {
        Weight = weight;
        Operation = operation;
        User = user;
    }

    public Repartition() { }
}