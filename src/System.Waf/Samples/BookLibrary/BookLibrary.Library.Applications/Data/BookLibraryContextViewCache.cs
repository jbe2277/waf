using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.MappingViews;
using Waf.BookLibrary.Library.Applications.Data;

[assembly: DbMappingViewCacheType(typeof(BookLibraryContext), typeof(BookLibraryContextViewCache))]

namespace Waf.BookLibrary.Library.Applications.Data
{
    internal sealed class BookLibraryContextViewCache : DbMappingViewCache
    {
        public override string MappingHashValue => "51e341e9b0527d94d51d362cb181d06bad45351153a7f7624de5c811a1d65d68";

        public override DbMappingView GetView(EntitySetBase extent)
        {
            if (extent == null) { throw new ArgumentNullException(nameof(extent)); }

            var extentName = extent.Name;

            if (extentName == "Person")
            {
                return GetPersonView();
            }
            else if (extentName == "Book")
            {
                return GetBookView();
            }
            else if (extentName == "People")
            {
                return GetPeopleView();
            }
            else if (extentName == "Books")
            {
                return GetBooksView();
            }
            else if (extentName == "Book_LendTo")
            {
                return GetBookLendToView();
            }

            return null;
        }

        private static DbMappingView GetPersonView()
        {
            return new DbMappingView("\r\n    SELECT VALUE -- Constructing Person\r\n        [CodeFirstDatabaseSchema.Person](T1.Person_Id, T1.Person_Firstname, T1.Person_Lastname, T1.Person_Email)\r\n    FROM (\r\n        SELECT \r\n            T.Id AS Person_Id, \r\n            T.Firstname AS Person_Firstname, \r\n            T.Lastname AS Person_Lastname, \r\n            T.Email AS Person_Email, \r\n            True AS _from0\r\n        FROM BookLibraryContext.People AS T\r\n    ) AS T1");
        }

        private static DbMappingView GetBookView()
        {
            return new DbMappingView("\r\n    SELECT VALUE -- Constructing Book\r\n        [CodeFirstDatabaseSchema.Book](T3.Book_Id, T3.Book_Title, T3.Book_Author, T3.Book_Publisher, T3.Book_PublishDate, T3.Book_Isbn, T3.Book_Language, T3.Book_Pages, T3.Book_PersonId)\r\n    FROM (\r\n        SELECT T1.Book_Id, T1.Book_Title, T1.Book_Author, T1.Book_Publisher, T1.Book_PublishDate, T1.Book_Isbn, T1.Book_Language, T1.Book_Pages, T2.Book_PersonId, T1._from0, (T2._from1 AND T2._from1 IS NOT NULL) AS _from1\r\n        FROM  (\r\n            SELECT \r\n                T.Id AS Book_Id, \r\n                T.Title AS Book_Title, \r\n                T.Author AS Book_Author, \r\n                T.Publisher AS Book_Publisher, \r\n                T.PublishDate AS Book_PublishDate, \r\n                T.Isbn AS Book_Isbn, \r\n                CAST(T.Language AS [Edm.Int32]) AS Book_Language, \r\n                T.Pages AS Book_Pages, \r\n                True AS _from0\r\n            FROM BookLibraryContext.Books AS T) AS T1\r\n            LEFT OUTER JOIN (\r\n            SELECT \r\n                Key(T.Book_LendTo_Source).Id AS Book_Id, \r\n                Key(T.Book_LendTo_Target).Id AS Book_PersonId, \r\n                True AS _from1\r\n            FROM BookLibraryContext.Book_LendTo AS T) AS T2\r\n            ON T1.Book_Id = T2.Book_Id\r\n    ) AS T3");
        }

        private static DbMappingView GetPeopleView()
        {
            return new DbMappingView("\r\n    SELECT VALUE -- Constructing People\r\n        [Waf.BookLibrary.Library.Applications.Data.Person](T1.Person_Id, T1.Person_Firstname, T1.Person_Lastname, T1.Person_Email)\r\n    FROM (\r\n        SELECT \r\n            T.Id AS Person_Id, \r\n            T.Firstname AS Person_Firstname, \r\n            T.Lastname AS Person_Lastname, \r\n            T.Email AS Person_Email, \r\n            True AS _from0\r\n        FROM CodeFirstDatabase.Person AS T\r\n    ) AS T1");
        }

        private static DbMappingView GetBooksView()
        {
            return new DbMappingView("\r\n    SELECT VALUE -- Constructing Books\r\n        [Waf.BookLibrary.Library.Applications.Data.Book](T1.Book_Id, T1.Book_Title, T1.Book_Author, T1.Book_Publisher, T1.Book_PublishDate, T1.Book_Isbn, T1.Book_Language, T1.Book_Pages) WITH \r\n        RELATIONSHIP(CREATEREF(BookLibraryContext.People, ROW(T1.[Book_LendTo.Book_LendTo_Target.Id]),[Waf.BookLibrary.Library.Applications.Data.Person]),[Waf.BookLibrary.Library.Applications.Data.Book_LendTo],Book_LendTo_Source,Book_LendTo_Target) \r\n    FROM (\r\n        SELECT \r\n            T.Id AS Book_Id, \r\n            T.Title AS Book_Title, \r\n            T.Author AS Book_Author, \r\n            T.Publisher AS Book_Publisher, \r\n            T.PublishDate AS Book_PublishDate, \r\n            T.Isbn AS Book_Isbn, \r\n            CAST(T.Language AS [Waf.BookLibrary.Library.Applications.Data.Language]) AS Book_Language, \r\n            T.Pages AS Book_Pages, \r\n            True AS _from0, \r\n            T.PersonId AS [Book_LendTo.Book_LendTo_Target.Id]\r\n        FROM CodeFirstDatabase.Book AS T\r\n    ) AS T1");
        }

        private static DbMappingView GetBookLendToView()
        {
            return new DbMappingView("\r\n    SELECT VALUE -- Constructing Book_LendTo\r\n        [Waf.BookLibrary.Library.Applications.Data.Book_LendTo](T3.[Book_LendTo.Book_LendTo_Source], T3.[Book_LendTo.Book_LendTo_Target])\r\n    FROM (\r\n        SELECT -- Constructing Book_LendTo_Source\r\n            CreateRef(BookLibraryContext.Books, row(T2.[Book_LendTo.Book_LendTo_Source.Id]), [Waf.BookLibrary.Library.Applications.Data.Book]) AS [Book_LendTo.Book_LendTo_Source], \r\n            T2.[Book_LendTo.Book_LendTo_Target]\r\n        FROM (\r\n            SELECT -- Constructing Book_LendTo_Target\r\n                T1.[Book_LendTo.Book_LendTo_Source.Id], \r\n                CreateRef(BookLibraryContext.People, row(T1.[Book_LendTo.Book_LendTo_Target.Id]), [Waf.BookLibrary.Library.Applications.Data.Person]) AS [Book_LendTo.Book_LendTo_Target]\r\n            FROM (\r\n                SELECT \r\n                    T.Id AS [Book_LendTo.Book_LendTo_Source.Id], \r\n                    T.PersonId AS [Book_LendTo.Book_LendTo_Target.Id], \r\n                    True AS _from0\r\n                FROM CodeFirstDatabase.Book AS T\r\n                WHERE T.PersonId IS NOT NULL\r\n            ) AS T1\r\n        ) AS T2\r\n    ) AS T3");
        }
    }
}
