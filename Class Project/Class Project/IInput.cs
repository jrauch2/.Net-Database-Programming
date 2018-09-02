﻿using System.Collections.Generic;

namespace Class_Project
{/// <summary>
/// The <c>IInput</c> interface.
/// provides signatures for methods to query stored tickets.
/// </summary>
    interface IInput
    {
        /// <summary>
        /// Get a <c>List<Ticket></c> of all stored tickets. 
        /// </summary>
        /// <returns>
        /// A <c>List<Ticket></c> of all stored tickets.
        /// </returns>
        List<Ticket> GetStoredTickets();
        /// <summary>
        /// Gets the highest ID stored.
        /// Used to generate new tickets without reusing IDs.
        /// </summary>
        /// <returns>
        /// The highest ID as an <c>int</c>.
        /// </returns>
        int GetMaxID();
        /// <summary>
        /// Get a <c>Ticket</c> by ID.
        /// </summary>
        /// <param name="id">The id of the desired <c>Ticket</c> as an <c></c>int</c>.</param>
        /// <returns>The desired <c>Ticket</c>, if found.</returns>
        Ticket FindId(int id);
    }
}