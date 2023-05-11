using EcommerceInterfaces;
using EcommerceInterfaces.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericSPController<T> : GenericController<T> where T : class
    {
        private readonly IStoreProcedures<T> storeProcedures;
        private readonly IAll<T> _all;

        public GenericSPController(IAll<T> allController, IStoreProcedures<T> storeProcedures) : base(allController)
        {
            this._all = allController;
            this.storeProcedures = storeProcedures;
        }

        //public GenericSPController(IAll<T> allController, IUser user, IStoreProcedures<T> storeProcedures, IAll<T> all) : base(allController, user)
        //{
        //    this.storeProcedures = storeProcedures;   
        //    this._all = all;
        //}




        //----------------------------- CREATE with Store Procedure
        [HttpPost]
        public async Task<IActionResult> CreateWithStoreProcedure(T entity)
        {
            try
            {
                var result = await storeProcedures.CreateWithSP(entity);
                if (result)
                {
                    await _all.CompleteAsync();
                    return Ok("Data Inserted Successfully");
                }
                return BadRequest("Data Inserted Failed...");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        // ------------------------------------- UPDATE WITH STORE PROCEDE
        [HttpPut]
        public async Task<IActionResult> UpdateWithStoreProcedure(T entity)
        {
            try
            {
                var result = await storeProcedures.UpdateWithSP(entity);
                if (result)
                {
                    await _all.CompleteAsync();
                    return Ok(entity);
                }
                return NotFound("Id Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        // ------------------------------------ DELETE

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveWithStoreProcedure(int id)
        {
            try
            {
                if (await storeProcedures.DeleteWithSP(id))
                {
                    await _all.CompleteAsync();
                    return Ok($"{id} Deleted Successfully");
                }
                return NotFound($"ID: {id} Not Exists");
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}
