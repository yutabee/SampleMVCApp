﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SampleMVCApp.Data;
using SampleMVCApp.Models;

namespace SampleMVCApp.Controllers
{
    public class PeopleController : Controller
    {
        private readonly SampleMVCAppContext _context;

        public PeopleController(SampleMVCAppContext context)
        {
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            //Viewの引数にオブジェクトを指定した場合、テンプレートの@Modelに引き渡される
            //通常デフォルトではページモデルがそのまま渡される
            //ToListAsyncは各レコードをインスタンス化してリストとして返す
            //(正確には非同期処理で\\Taskインスタンスとして返す)
              return _context.Person != null ? 
                          View(await _context.Person.ToListAsync()) : 
                          Problem("Entity set 'SampleMVCAppContext.Person'  is null.");
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Person == null)
            {
                return NotFound();
            }
            //FirstOrDefaultAsyncは一致した最初の要素を返す
            //一致しなかった場合はnullを返す（エラーにならない）
            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.PersonId == id);
            //nullだった場合Notfoundを返す
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]　//POST送信を受け取る
        [ValidateAntiForgeryToken] // XSRF攻撃対策のため
        public async Task<IActionResult> Create([Bind("PersonId,Name,Mail,Age")] Person person)
        {
            //フォームから送信された内容をバインドを用いいてPersonインスタンスとして受け取る
            //バリデーションのチェック
            if (ModelState.IsValid)
            {
                //問題なければPersonインスタンスをデータベースコンテキストに追加して保存
                _context.Add(person);
                await _context.SaveChangesAsync();
                //インデックスアクションの名前とパスに指定してリダイレクト
                return RedirectToAction(nameof(Index));
            }
            //取得したインスタンスをDetailのViewに渡す
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Person == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonId,Name,Mail,Age")] Person person)
        {
            if (id != person.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Person == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Person == null)
            {
                return Problem("Entity set 'SampleMVCAppContext.Person'  is null.");
            }
            var person = await _context.Person.FindAsync(id);
            if (person != null)
            {
                _context.Person.Remove(person);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
          return (_context.Person?.Any(e => e.PersonId == id)).GetValueOrDefault();
        }
    }
}
