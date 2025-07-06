using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WordTranslationApp;
using WordTranslationApp.Models;

namespace WortSchatz.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordsController : ControllerBase
    {
        private readonly WordRepository _wordRepository;

        public WordsController(WordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WordDto>>> GetWords()
        {
            var words = await _wordRepository.GetAllAsync();
            var wordDtos = words.Select(w => w.ToDto());

            return Ok(wordDtos);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<WordDto>> CreateWord(WordDto wordDto)
        {
            var word = Word.FromDto(wordDto);
            await _wordRepository.AddAsync(word);

            return CreatedAtAction(nameof(GetWords), new { id = word.Id }, word);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllWords()
        {
            await _wordRepository.DeleteAllAsync();
            return NoContent();
        }
    }
}
