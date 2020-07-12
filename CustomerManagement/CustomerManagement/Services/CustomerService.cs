﻿using CustomerManagement.DTO;
using CustomerManagement.Interfaces;
using CustomerManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagement.Services
{
    public class CustomerService : ICustomerService
    {
        #region Field

        private readonly ICustomerRepository _customerRepository;
        private readonly ILoggingService _logger;

        #endregion

        #region Constructor

        public CustomerService(ICustomerRepository customerRepository, ILoggingService logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        #endregion

        #region Method

        /// <summary>
        /// Thêm mới khách hàng
        /// </summary>
        /// <param name="customer">Đối tượng khách hàng</param>
        /// <returns>true: Thêm mới thành công, false: Thêm mới thất bại</returns>
        /// Created by: TMSANG (03/07/2020)
        public async Task<Customer> CreateAsync(Customer customer)
        {
            var customerDTO = new CustomerDTO
            {
                FullName = customer.FullName
            };
            var createdCustomer = await _customerRepository.CreateAsync(customerDTO);
            _logger.LogInformation("Đã thêm một khách hàng mới với Id: {Id}", createdCustomer.Id);
            return MapDtoToDomain(createdCustomer);
        }

        /// <summary>
        /// Lấy thông tin của tất cả khách hàng
        /// </summary>
        /// <returns>Danh sách tất cả khách hàng</returns>
        /// Created by: TMSANG (03/07/2020)
        public async Task<List<Customer>> GetAllAsync()
        {
            var customerDTOs = (await _customerRepository.GetAllAsync()).ToList();
            _logger.LogInformation("Đã get được {Count} khách hàng", customerDTOs.Count);
            return customerDTOs.Select(MapDtoToDomain).ToList();
        }

        /// /// <summary>
        /// Lấy thông tin khách hàng theo Id
        /// </summary>
        /// <param name="id">Id của khách hàng</param>
        /// <returns>Thông tin khách hàng theo Id</returns>
        /// Created by: TMSANG (03/07/2020)
        public async Task<Customer> GetByIdAsync(Guid customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                _logger.LogInformation("Không tìm thấy khách hàng với Id: {Id}", customerId);
                return null;
            }

            _logger.LogInformation("Đã get được một khách hàng với Id: {Id}", customerId);
            return MapDtoToDomain(customer);
        }

        private Customer MapDtoToDomain(CustomerDTO dto)
        {
            return new Customer
            {
                Id = Guid.Parse(dto.Id),
                FullName = dto.FullName
            };
        }
        #endregion
    }
}
