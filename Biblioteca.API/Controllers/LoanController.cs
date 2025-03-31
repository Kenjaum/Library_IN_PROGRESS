﻿using Library.Application.Commands.CreateLoan;
using Library.Application.Queries.GetAllLoans;
using Library.Application.Queries.GetLoanById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/v1/Loan")]
    public class LoanController : ControllerBase
    {
        [Route("api/[controller]")]
        [ApiController]
        public class LoansController : ControllerBase
        {
            private readonly IMediator _mediator;

            public LoansController(IMediator mediator)
            {
                _mediator = mediator;
            }
            [HttpGet]
            [Authorize(Roles = "manager, student")]
            public async Task<IActionResult> GetAll()
            {
                var getAllLoansQuery = new GetAllLoansQuery();

                var loan = await _mediator.Send(getAllLoansQuery);

                return Ok(loan);
            }

            [HttpGet("{id}")]
            [Authorize(Roles = "manager, student")]
            public async Task<IActionResult> GetById(int id)
            {
                var query = new GetLoanByIdQuery(id);

                var loan = await _mediator.Send(query);

                if (loan == null)
                {
                    return NotFound();
                }

                return Ok(loan);

            }

            [HttpPost]
            [Authorize(Roles = "manager, student")]
            public async Task<IActionResult> Post([FromBody] CreateLoanCommand command)
            {
                await _mediator.Send(command);

                return NoContent();
            }
        }
    }
}
