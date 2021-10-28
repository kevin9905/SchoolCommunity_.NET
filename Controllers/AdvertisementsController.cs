using Lab4.Models.ViewModels;
using Lab4.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Azure;
using Assignment1.Models;
using System.IO;
using Assignment1.Models.ViewModels;
using Lab4.Data;

namespace Assignment1.Controllers
{
    public class AdvertisementsController : Controller
    {
        private readonly SchoolCommunityContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string containerName = "advertisement";
  

        public AdvertisementsController(SchoolCommunityContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public async Task<IActionResult> Index(string id)
        {

            var view = new AdsViewModel();
            view.Advertisements = await _context.Advertisements.Where(c => c.CommunityID == id)
                .OrderBy(c => c.AdvertisementUrl)
                .AsNoTracking()
                .ToListAsync();
            view.Community = _context.Communities.Where(m => m.id == id).Single();

            return View(view);
            


        }

        public async Task<IActionResult> UploadAsync(string id)
        {
         

            var view = new AdsViewModel();
            view.Advertisements = await _context.Advertisements.Where(a => a.CommunityID == id)
                .OrderBy(c => c.AdvertisementUrl)
                .AsNoTracking()
                .ToListAsync();
            view.Community = _context.Communities.Where(a => a.id == id).Single();

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file,string id)
        {

            BlobContainerClient containerClient;
                // Create the container and return a container client object
                try
                {
                    containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName);
                    containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
                }
                catch (RequestFailedException)
                {
                    containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                }


                try
                {
                    // create the blob to hold the data
                    var blockBlob = containerClient.GetBlobClient(file.FileName);
                    if (await blockBlob.ExistsAsync())
                    {
                        await blockBlob.DeleteAsync();
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        // copy the file data into memory
                        await file.CopyToAsync(memoryStream);

                        // navigate back to the beginning of the memory stream
                        memoryStream.Position = 0;

                        // send the file to the cloud
                        await blockBlob.UploadAsync(memoryStream);
                        memoryStream.Close();
                    }

                    // add the photo to the database if it uploaded successfully
                    var image = new Advertisement();
                image.CommunityID = id;
                image.AdvertisementUrl = blockBlob.Uri.AbsoluteUri;
                image.FileName = file.FileName;

                    _context.Advertisements.Add(image);
                    _context.SaveChanges();
                }
                catch (RequestFailedException)
                {
                    View("Error");
                }

            return RedirectToAction("Index", new { id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Advertisements
                .FirstOrDefaultAsync(m => m.AdvertisementId == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id,string checkid)
        {
            var image = await _context.Advertisements.FindAsync(id);


            BlobContainerClient containerClient;
            // Get the container and return a container client object
            try
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }
            catch (RequestFailedException)
            {
                return View("Error");
            }

            try
            {
                // Get the blob that holds the data
                var blockBlob = containerClient.GetBlobClient(image.FileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                _context.Advertisements.Remove(image);
                await _context.SaveChangesAsync();

            }
            catch (RequestFailedException)
            {
                return View("Error");
            }

            return RedirectToAction("Index", new {checkid});
        }

    }
}
