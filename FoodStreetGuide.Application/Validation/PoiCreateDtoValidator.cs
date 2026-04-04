using FoodStreetGuide.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace FoodStreetGuide.Application.Validation

    public class PoiCreateDtoValidator : AbstractValidator<PoiCreateDto>

{
    internal class PoiCreateDtoValidator
    {
        public PoiCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên POI không được để trống")
                .MaximumLength(200).WithMessage("Tên POI tối đa 200 ký tự");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Mô tả tối đa 2000 ký tự");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Vĩ độ phải từ -90 đến 90");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Kinh độ phải từ -180 đến 180");

            RuleFor(x => x.RadiusMeters)
                .GreaterThan(0).WithMessage("Bán kính geofence phải lớn hơn 0");

            RuleFor(x => x.Priority)
                .GreaterThanOrEqualTo(1).WithMessage("Ưu tiên phải lớn hơn hoặc bằng 1");
        }
    }
}
