﻿using Business.Abstract;
using Core.Entities.Utilities.Results;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using static Core.Entities.Utilities.Results.IDataResult;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public IDataResult<List<Category>> GetAll()
        {
            //İş kodları
            return new SuccessDataResult<List<Category>>(_categoryDal.GetAll());
        }

        //Select * from Categories where CategoryId = 3
        public IDataResult<Category> GetById(int categoryId)
        {
            return new SuccessDataResult<Category>(_categoryDal.Get(c => c.CategoryId == categoryId));
        }

        List<Category> ICategoryService.GetAll()
        {
            throw new NotImplementedException();
        }

        List<Category> ICategoryService.GetById(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}