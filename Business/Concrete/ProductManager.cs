﻿using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Utilities.Results;
using Core.Utilities.Business;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Core.Entities.Utilities.Results.IDataResult;


namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;
        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }
        //Operation=methods
        //Claim = administry
        [SecuredOperation("admin,product.add")]
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName),
                 CheckIfProductCountofCategoryCorrect(product.CategoryId), CheckIfCategoryLimitExceeded());

            if (result != null)
            {
                return result;
            }



            if (CheckIfProductCountofCategoryCorrect(product.CategoryId).Success)
            {
                if (CheckIfProductNameExists(product.ProductName).Success)
                {
                    _productDal.Add(product);
                    return new SuccessResult(Messages.ProductAdded);
                }


            }



            return new ErrorResult();


        }


        [CacheAspect]// key,value
        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 1)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        public SuccessDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }
        public SuccessDataResult<List<Product>> GetById(int productId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.ProductId == productId));
        }
        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        public IResult Update(Product product)
        {
            throw new NotImplementedException();
        }

        IDataResult<List<Product>> IProductService.GetAllByCategoryId(int id)
        {
            throw new NotImplementedException();
        }

        Product IProductService.GetById(int productId) {
            throw new NotImplementedException();
        }
        private IResult CheckIfProductCountofCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result >= 15)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceeded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceeded);
            }
            return new SuccessResult();
        }

        public IResult AddTransactionalTest(Product product)
        {
            throw new NotImplementedException();
        }
    }
}