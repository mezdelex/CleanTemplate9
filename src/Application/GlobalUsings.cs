global using Application.Abstractions;
global using Application.Features.Commands;
global using Application.Features.DomainEvents;
global using Application.Features.Shared;
global using Application.Messages;
global using Application.Repositories;
global using Application.Requests;
global using Ardalis.Specification;
global using AutoMapper;
global using Domain.Cache;
global using Domain.Entities;
global using Domain.Exceptions;
global using static Domain.Extensions.Collections;
global using Domain.Identity;
global using Domain.Persistence;
global using Domain.Specifications;
global using FluentValidation;
global using MassTransit;
global using MediatR;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
