using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_MachineSevice.Models;

namespace WPF_MachineSevice.Repository
{
    public class UnitOfWork : IDisposable
    {
        private ScanMachineContext context = new ScanMachineContext();
       
        private GenericRepository<Category> categoryRepository;
        
        private GenericRepository<Order> orderRepository;

        private GenericRepository<OrderDetail> orderDetailRepositoty;

        private GenericRepository<OrderImage> orderImageRepositoty;
     
        private GenericRepository<Product> productRepository;

        private GenericRepository<Image> imageRepository;

        public GenericRepository<Category> CategoryRepository
        {
            get
            {

                if (categoryRepository == null)
                {
                    categoryRepository = new GenericRepository<Category>(context);
                }
                return categoryRepository;
            }
        }
       
        public GenericRepository<Order> OrderRepository
        {
            get
            {

                if (orderRepository == null)
                {
                    orderRepository = new GenericRepository<Order>(context);
                }
                return orderRepository;
            }
        }
        public GenericRepository<OrderDetail> OrderDetailRepository
        {
            get
            {

                if (orderDetailRepositoty == null)
                {
                    orderDetailRepositoty = new GenericRepository<OrderDetail>(context);
                }
                return orderDetailRepositoty;
            }
        }
        public GenericRepository<OrderImage> OrderImageRepository
        {
            get
            {

                if (orderImageRepositoty == null)
                {
                    orderImageRepositoty = new GenericRepository<OrderImage>(context);
                }
                return orderImageRepositoty;
            }
        }

        public GenericRepository<Product> ProductRepository
        {
            get
            {

                if (productRepository == null)
                {
                    productRepository = new GenericRepository<Product>(context);
                }
                return productRepository;
            }
        }
        public GenericRepository<Image> ImageRepository
        {
            get
            {

                if (imageRepository == null)
                {
                    imageRepository = new GenericRepository<Image>(context);
                }
                return imageRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
