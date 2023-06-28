﻿using System;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Model.DTO
{
	public class VillaNumberCreateDTO
	{
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaID { get; set; }
        public string SpecialDetails { get; set; }
    }
}

