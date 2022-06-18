using Core.Entity;
using Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    internal class UnitOfWork : IUnitOfWork
    {
        private Infrastructure.Data.SolidoDbContext _context;
        private IRepository<Roof> roofRepository;
        private IRepository<Contact> contactRepository;
        private IRepository<Main> mainsRepository;

        public UnitOfWork(Infrastructure.Data.SolidoDbContext context)
        {
            _context = context;
        }

        public IRepository<Roof> RoofRepository
        {
            get
            {
                if (roofRepository == null)
                    roofRepository = new BaseRepository<Roof>(_context);
                return roofRepository;
            }
        }

        public IRepository<Contact> ContactRepository
        {
            get
            {
                if (contactRepository == null)
                    contactRepository = new BaseRepository<Contact>(_context);
                return contactRepository;
            }
        }

        public IRepository<Main> MainsRepository
        {
            get
            {
                if (mainsRepository == null)
                    mainsRepository = new BaseRepository<Main>(_context);
                return mainsRepository;
            }
        }

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
