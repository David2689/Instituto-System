using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace YourAppNamespace.Pages
{
    public class MatriculaModel : PageModel
    {
        // --- Informações da criança ---

        [BindProperty]
        [Required(ErrorMessage = "Informe o nome completo da criança.")]
        [Display(Name = "Nome completo")]
        public string FullName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Informe a data de nascimento.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de nascimento")]
        public DateTime? Birth { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Selecione o sexo.")]
        [Display(Name = "Sexo")]
        public string Gender { get; set; }

        [BindProperty(Name = "medical-info")]
        [Display(Name = "Informações médicas")]
        public string MedicalInfo { get; set; }

        [BindProperty(Name = "birth-file")]
        [Display(Name = "Certidão de Nascimento")]
        public IFormFile BirthFile { get; set; }

        // --- Endereço residencial ---

        [BindProperty]
        [Required(ErrorMessage = "Informe o CEP.")]
        [Display(Name = "CEP")]
        public string Cep { get; set; }

        [BindProperty]
        [Display(Name = "Rua")]
        public string Street { get; set; }

        [BindProperty]
        [Display(Name = "Número")]
        public int? Number { get; set; }

        [BindProperty]
        [Display(Name = "Cidade")]
        public string City { get; set; }

        [BindProperty]
        [Display(Name = "Estado")]
        public string State { get; set; }

        // --- Responsável ---

        [BindProperty]
        [Required(ErrorMessage = "Informe o nome do responsável.")]
        [Display(Name = "Nome do responsável")]
        public string Gardian { get; set; }

        [BindProperty]
        [Display(Name = "Telefone")]
        public string Phone { get; set; }

        [BindProperty(Name = "mail")]
        [Required(ErrorMessage = "Informe um e-mail válido.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        [Display(Name = "Email")]
        public string Mail { get; set; }

        // --- Opções de matrícula ---

        [BindProperty(Name = "study-shift")]
        [Required(ErrorMessage = "Selecione o turno de estudo.")]
        [Display(Name = "Turno de estudo")]
        public string StudyShift { get; set; }

        [BindProperty]
        [Display(Name = "Esporte")]
        public string Sport { get; set; }

        // --- Termos ---

        [BindProperty]
        [Display(Name = "Aceito os termos")]
        public bool Terms { get; set; }

        // Mensagem de feedback para a tela
        [TempData]
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            // Se quiser preencher valores padrão, faça aqui.
            Street = "Rua das Flores";
            City = "São Paulo";
            State = "SP";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validação de termos manual (para garantir que marcou o checkbox)
            if (!Terms)
            {
                ModelState.AddModelError(nameof(Terms), "Você deve aceitar os termos para concluir a matrícula.");
            }

            if (!ModelState.IsValid)
            {
                // Volta para a página mostrando erros de validação
                return Page();
            }

            // Exemplo de gravação do arquivo enviado (certidão de nascimento)
            if (BirthFile != null && BirthFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(BirthFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await BirthFile.CopyToAsync(stream);
                }

                // Aqui você poderia salvar o caminho no banco de dados, se quiser
            }

            // Aqui é onde você salvaria todos os dados da matrícula no banco de dados

            SuccessMessage = "Matrícula realizada com sucesso!";

            // Pode retornar RedirectToPage para evitar repost do formulário
            return RedirectToPage();
        }
    }
}
