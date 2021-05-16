using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeAreFamily.MemberIdentity.Api.Data;
using WeAreFamily.Models.MemberIdentity;

namespace WeAreFamily.MemberIdentity.Api.Controllers
{
    public class MembersController : Controller
    {
        private readonly MemberIdentityContext _context;

        public MembersController(MemberIdentityContext context)
        {
            _context = context;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string userName, string password, int key)
        {
            var decPass = Encryptor_.Decrypt(password, (CodeKey_)key);
            var member = _context.Members.FirstOrDefault(m => m.UserName == userName && m.Password == decPass);

            return null;

            //try
            //{

            //}
            //catch (Exception e)
            //{
            //    return HttpStatus
            //}
        }

        //// GET: Members
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Members.ToListAsync());
        //}

        //// GET: Members/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var member = await _context.Members
        //        .FirstOrDefaultAsync(m => m.MembershipId == id);
        //    if (member == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(member);
        //}

        //// GET: Members/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Members/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("MembershipId,NationalityId,FirstName,MiddleName,LastName,UserName,Password")] Member member)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(member);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(member);
        //}

        //// GET: Members/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var member = await _context.Members.FindAsync(id);
        //    if (member == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(member);
        //}

        //// POST: Members/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("MembershipId,NationalityId,FirstName,MiddleName,LastName,UserName,Password")] Member member)
        //{
        //    if (id != member.MembershipId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(member);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MemberExists(member.MembershipId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(member);
        //}

        //// GET: Members/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var member = await _context.Members
        //        .FirstOrDefaultAsync(m => m.MembershipId == id);
        //    if (member == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(member);
        //}

        //// POST: Members/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var member = await _context.Members.FindAsync(id);
        //    _context.Members.Remove(member);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool MemberExists(int id)
        //{
        //    return _context.Members.Any(e => e.MembershipId == id);
        //}
    }

    public class Encryptor_
    {
        public static string Encrypt(string decryption, CodeKey_ decodeKey)
        {
            var encryption = string.Empty;
            var temp = string.Empty;
            var addition = string.Empty;

            switch (decodeKey)
            {
                case CodeKey_.oei_ntla_ltnb_si:
                    decryption.ToList().ForEach(c =>
                    {
                        var charString = $"{string.Empty}{c}";

                        if (Char.IsNumber(c))
                            addition = TranslatedNumber(charString, decodeKey);
                        else if (Char.IsLetter(c))
                            addition = TranslatedLetter(charString, decodeKey, above: false);
                        else addition = charString;

                        temp += string.Join(string.Empty, Enumerable.Range(1, (int)decodeKey).Select(n => addition));
                    });
                    encryption = temp += (int)decodeKey;
                    break;
                case CodeKey_.pi_oei_ntlb_ltna:
                    decryption.ToList().ForEach(c =>
                    {
                        var charString = $"{string.Empty}{c}";

                        if (Char.IsNumber(c))
                            addition = TranslatedNumber(charString, decodeKey, above: false);
                        else if (Char.IsLetter(c))
                            addition = TranslatedLetter(charString, decodeKey);
                        else addition = charString;

                        temp += string.Join(string.Empty, Enumerable.Range(1, (int)decodeKey).Select(n => addition));
                    });
                    encryption = $"{(int)decodeKey} + {temp}";
                    break;
            }

            return encryption;
        }

        private static string TranslatedNumber(string charString, CodeKey_ codeKey, bool above = true)
        {
            var number = above ? int.Parse(charString) + (int)codeKey : int.Parse(charString) - (int)codeKey;
            var translation = Enum.GetValues(typeof(TranslationDescription_)).Cast<TranslationDescription_>().ToList().Find(e => (int)e == number);
            return EnumDescription(translation);
        }
        private static string TranslatedLetter(string charString, CodeKey_ codeKey, bool above = true)
        {
            var letter = charString;
            var translation = Enum.GetValues(typeof(TranslationDescription_)).Cast<TranslationDescription_>().ToList().Find(e => EnumDescription(e) == letter);
            var number = above ? (int)translation + (int)codeKey : (int)translation - (int)codeKey;
            return number.ToString();
        }

        private static string EnumDescription(TranslationDescription_ translation)
        {
            return ((DescriptionAttribute)translation.GetType().GetMember(translation.ToString())[0].GetCustomAttributes(typeof(DescriptionAttribute), false).ElementAt(0)).Description;
        }

        public static string Decrypt(string encryption, CodeKey_ encodeKey)
        {
            var decryption = string.Empty;
            var temp = string.Empty;
            List<char> distinctEncryption = null;
            var addition = string.Empty;

            switch (encodeKey)
            {
                case CodeKey_.oei_ntla_ltnb_si:
                    temp = encryption.TrimEnd($"{(int)encodeKey}".ToArray());
                    distinctEncryption = temp.GroupBy(g => g).Select(c => c.Key).ToList();

                    distinctEncryption.ToList().ForEach(c =>
                    {
                        var charString = $"{string.Empty}{c}";

                        if (Char.IsNumber(c))
                            addition = TranslatedNumber(charString, encodeKey, above: false);
                        else if (Char.IsLetter(c))
                            addition = TranslatedLetter(charString, encodeKey);
                        else addition = charString;

                        decryption += addition;
                    });
                    break;
                case CodeKey_.pi_oei_ntlb_ltna:
                    temp = encryption.TrimStart($"{(int)encodeKey}".ToArray());
                    distinctEncryption = temp.GroupBy(g => g).Select(c => c.Key).ToList();

                    distinctEncryption.ToList().ForEach(c =>
                    {
                        var charString = $"{string.Empty}{c}";

                        if (Char.IsNumber(c))
                            addition = TranslatedNumber(charString, encodeKey);
                        else if (Char.IsLetter(c))
                            addition = TranslatedLetter(charString, encodeKey, above: false);
                        else addition = charString;

                        decryption += addition;
                    });
                    break;
            }

            return decryption;
        }
    }

    public enum CodeKey_
    {
        [Description("one equals {id}, number to letter ({id} times above), letter to number ({id} times bellow), suffix {id}")]
        oei_ntla_ltnb_si = 2,
        [Description("prefix {id}, one equals {id}, number to letter ({id} times bellow), letter to number ({id} times above)")]
        pi_oei_ntlb_ltna = 3
    }

    internal enum TranslationDescription_
    {
        [Description("F")]
        A = 0,
        [Description("I")]
        B = 1,
        [Description("H")]
        C = 2,
        [Description("B")]
        D = 3,
        [Description("E")]
        E = 4,
        [Description("A")]
        F = 5,
        [Description("C")]
        G = 6,
        [Description("D")]
        H = 7,
        [Description("J")]
        I = 8,
        [Description("G")]
        J = 9
    }
}
