using LibraryManagementBackend.Models;
using LibraryManagementBackend.Services;
using LibraryManagementBackend.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementBackend.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;
        private readonly ILogger<PublisherService> _logger;

        public PublisherService(IPublisherRepository publisherRepository, ILogger<PublisherService> logger)
        {
            _publisherRepository = publisherRepository;
            _logger = logger;
        }


        public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all publishers");
                return await _publisherRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all publishers");
                throw;
            }
        }


        public async Task<Publisher?> GetPublisherByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching publisher with ID: {PublisherId}", id);
                return await _publisherRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching publisher with ID: {PublisherId}", id);
                throw;
            }
        }


        public async Task<Publisher> CreatePublisherAsync(Publisher publisher)
        {
            try
            {
                _logger.LogInformation("Creating new publisher: {PublisherName}", publisher.Name);

                // business logic validation
                var existingPublisher = await _publisherRepository.GetByNameAsync(publisher.Name);
                if (existingPublisher != null)
                {
                    throw new InvalidOperationException($"Publisher with name '{publisher.Name}' already exists");
                }

                var createdPublisher = await _publisherRepository.AddAsync(publisher);

                _logger.LogInformation("Publisher created successfully with ID: {PublisherId}", createdPublisher.Id);
                return createdPublisher;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating publisher: {PublisherName}", publisher.Name);
                throw;
            }
        }


        public async Task<Publisher> UpdatePublisherAsync(int id, Publisher publisher)
        {
            try
            {
                _logger.LogInformation("Updating publisher with ID: {PublisherId}", id);

                var existingPublisher = await _publisherRepository.GetByIdAsync(id);
                if (existingPublisher == null)
                {
                    throw new KeyNotFoundException($"Publisher with ID {id} not found");
                }

                // check for duplicate name (excluding current publisher)
                var duplicatePublisher = await _publisherRepository.GetByNameAsync(publisher.Name);
                if (duplicatePublisher != null && duplicatePublisher.Id != id)
                {
                    throw new InvalidOperationException($"Another publisher with name '{publisher.Name}' already exists");
                }

                existingPublisher.Name = publisher.Name;
                existingPublisher.Address = publisher.Address;

                await _publisherRepository.UpdateAsync(existingPublisher);

                _logger.LogInformation("Publisher updated successfully with ID: {PublisherId}", id);
                return existingPublisher;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating publisher with ID: {PublisherId}", id);
                throw;
            }
        }

        public async Task<bool> DeletePublisherAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting publisher with ID: {PublisherId}", id);

                var exists = await _publisherRepository.ExistsAsync(id);
                if (!exists)
                {
                    return false;
                }

                await _publisherRepository.DeleteAsync(id);

                _logger.LogInformation("Publisher deleted successfully with ID: {PublisherId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting publisher with ID: {PublisherId}", id);
                throw;
            }
        }
        
        public async Task<bool> PublisherExistsAsync(int id)
        {
            return await _publisherRepository.ExistsAsync(id);
        }

    }
}



