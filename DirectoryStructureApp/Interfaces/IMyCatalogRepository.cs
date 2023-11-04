﻿using DirectoryStructureApp.Models;

namespace DirectoryStructureApp.Interfaces
{
    public interface IMyCatalogRepository
    {

        Task AddListCatalogsAsync(List<MyCatalog> catalogs);
        Task<List<MyCatalog>> GetAllAsync();
        Task DeleteAllMyCatalogsAsync();
    }
}
