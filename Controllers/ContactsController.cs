using ContactsApi.Data;
using ContactsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Loader;

namespace ContactsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {

        private readonly ContactsAPIDbContext dbContext;

        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]

        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> AddContacts(AddRequestContact  addRequestContact)
        {

            var contact = new Contact()
            { 
                Id = Guid.NewGuid(),
                FullName = addRequestContact.FullName,
                Address = addRequestContact.Address,
                Email = addRequestContact.Email,
                Phone = addRequestContact.Phone
            };
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {

            var contact = await dbContext.Contacts.FindAsync(id);

            if(contact == null)
                return NotFound();

            contact.FullName= updateContactRequest.FullName;
            contact.Address= updateContactRequest.Address;
            contact.Email= updateContactRequest.Email;
            contact.Phone= updateContactRequest.Phone;

            await dbContext.SaveChangesAsync();

            return Ok(contact);
           
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }
            dbContext.Contacts.Remove(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);
        }

    }
}
