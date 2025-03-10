﻿using MediatR;

namespace PruebatecnicaBack.Application.Scraper.Commands.GetData;

public record GetDataCommand(int UserId, int Year, bool Update) : IRequest<string>;
